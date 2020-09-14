# Preparation

Project was based on[ Raspberry Pi 4B](https://www.raspberrypi.org/products/raspberry-pi-4-model-b/ " Raspberry Pi 4B") with 2GB of RAM.
Download 64x ARM Linux from [Ubuntu](http://https://ubuntu.com/download/raspberry-pi "Ubuntu website"), then flash it to your SD card.


# Linux Setup
Default password and username: ubuntu

Sometimes trying to log in directly on Raspberry does not work, but it is still possible to login via SSH. To do that, start CMD on your windows machine, connect the Raspberry to your local network and in your CMD write: `ssh ubuntu@<raspberry ip>`. You can find IP by going to your router settings or scanning your network with external tools.
In my case IP was `192.168.0.25` so the command looked like: `ssh ubuntu@192.168.0.25`.
After connecting you will be prompted for password and forced to change it after succesful login.

### Configure Wireless Network

Next three sections are based on a great guide found on [linuxbabe.com](https://www.linuxbabe.com/ubuntu/connect-to-wi-fi-from-terminal-on-ubuntu-18-04-19-04-with-wpa-supplicant "linuxbabe.com"). If you'd like to know more about specific steps please refer to the guide on that website.

- First of all install net-tools: `sudo apt-get install net-tools` and wireless-tools: `sudo apt-get install wireless-tools`
- Turn on your wirless adapter: `sudo ifconfig wlan0 up`
- Scan available networks: `sudo iwlist wlan0 scan`
- Find your Wi-Fi on the list and remember the ESSID, for example: `ESSID:"5G_FooBar"`
- Create new folder to store settings(you might want to think about where you want to store them if you're worried about security): `mkdir configs`
- Create your wireless network credential settings with ESSID you found earlier: `wpa_passphrase "5G_FooBar" password > configs/wpa_config.conf`
- It should look like that, you're free to delete #psk line:
[![WPA_PASSPHRASE example](https://i.imgur.com/f2kZ3H5.png "WPA_PASSPHRASE example")](https://i.imgur.com/f2kZ3H5.png "WPA_PASSPHRASE example")
- Check if everything is as it should be: 
	- Run it with `sudo wpa_supplicant -cyour_path_to_file/wpa_config.conf -iwlan0`
	- You should see something similiar to `CTRL-EVENT-CONNECTED - Connection to 11:22:33:44:55:66 completed`
	- Press CTRL+C to stop it.
- Now run it again in the background by using the same command but adding -B parameter, e.g.: `sudo wpa_supplicant -B -cyour_path_to_file/wpa_config.conf -iwlan0`
- Request IP from your router: `sudo dhclient wlan0`
-  If everything went correctly your wlan0 should have an IP. Check it with: `ifconfig wlan0`

Congratulations, your Wi-Fi is nearly ready :smile:

### Connect to Wi-Fi on boot

- Copy the required file: `sudo cp /lib/systemd/system/wpa_supplicant.service /etc/systemd/system/wpa_supplicant.service`
- Edit copied file: `sudo nano /etc/systemd/system/wpa_supplicant.service`
- Change line `ExecStart=/sbin/wpa_supplicant -u -s -O /run/wpa_supplicant` to: `ExecStart=/sbin/wpa_supplicant -u -s -c your_path_to_file/wpa_config.conf -i wlan0`
- Comment `Alias=dbus-fi.w1.wpa_supplicant1.service` by adding # at the beginning and save the file.
- Start the wpa_supplicant service: `sudo systemctl enable wpa_supplicant.service`
- Create dhclient.service: `sudo nano /etc/systemd/system/dhclient.service`
- Paste below configuration inside and save the file.

```
[Unit]
Description= DHCP Client
Before=network.target
After=wpa_supplicant.service

[Service]
Type=simple
ExecStart=/sbin/dhclient wlan0 -v
ExecStop=/sbin/dhclient wlan0 -r

[Install]
WantedBy=multi-user.target
```

- Start the dhclient service: `sudo systemctl enable dhclient.service`

If everything was done correctly Wi-Fi should connect automatically after boot!
If it doesn't have IP after restarting Raspberry I recommend turning off the dhclient.service.

### Custom hostname

- First install the daemon: `sudo apt-get install avahi-daemon`
- Turn the daemon service on: `sudo systemctl enable avahi-daemon`
- Set custom hostname for your Pi: `sudo hostnamectl set-hostname raspberry`
- Restart the daemon for new changes: `sudo systemctl restart avahi-daemon`

Your raspberry should be now available under **raspberry.local**.

### FTP Server

- First let's install FTP server for easy way to transfer files.
- Download and install proftpd: `sudo apt-get install proftpd`
- Try connecting to it with FTP client, for example [FileZilla](https://filezilla-project.org/ "FileZilla"). Example:
[![FileZilla connection example](https://i.imgur.com/IdBb4nW.png "FileZilla connection example")](https://i.imgur.com/IdBb4nW.png "FileZilla connection example")

### Setting up  .NET environment

Next two sections are borrowed from [edi.wang](https://edi.wang/post/2019/9/29/setup-net-core-30-runtime-and-sdk-on-raspberry-pi-4 "edi.wang"). If you'd like to know more about specific steps please refer to the guide on that website.

- Create separate folder for your .NET files: `mkdir dotnet_arm64`
- Download .NET Core SDK and ASP.NET Core Runtime from [microsoft.com](https://dotnet.microsoft.com/download/dotnet-core/3.1 "microsoft.com"). Example: `wget https://download.visualstudio.microsoft.com/download/pr/5d8bf507-759a-4cc6-92ae-8ef63478398a/6b298aad0f6ce04ebc09daa1007a4248/aspnetcore-runtime-3.1.7-linux-arm64.tar.gz`
- Unpack files to folder created earlier, for example: `tar zxf dotnet-sdk-3.1.401-linux-arm64.tar.gz -C /home/ubuntu/dotnet_arm64`. Both should go to the same folder.
- Enter the user(e.g. /home/ubuntu/) folder and edit .profile file: `nano .profile`. Paste the following at the end:

```
export DOTNET_ROOT=$HOME/dotnet_arm64
export PATH=$PATH:$HOME/dotnet_arm64
```

After a reboot everything should work.

### Setting up nginx

- Install nginx: `sudo apt-get install nginx`
- Start nginx: `sudo sudo /etc/init.d/nginx start`
- Open nginx configuration file: `sudo nano /etc/nginx/sites-available/default` and replace it's contents with:

```
server {
    listen        80 default_server;
    server_name   _;
    location / {
        proxy_pass         http://localhost:5000;
        proxy_http_version 1.1;
        proxy_set_header   Upgrade $http_upgrade;
        proxy_set_header   Connection keep-alive;
        proxy_set_header   Host $host;
        proxy_cache_bypass $http_upgrade;
        proxy_set_header   X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header   X-Forwarded-Proto $scheme;
    }
}
```
- Check if configuration files has no errors: `sudo nginx -t`
- Apply new settings: `sudo nginx -s reload`

Your website should now be accessible from port 80.

### Making server start after crash/reboot

- Create new service file: `sudo nano /etc/systemd/system/iot_server.service`
- Paste below content:

```
[Unit]
Description=IoT Raspberry Server

[Service]
WorkingDirectory=/home/ubuntu/server/
ExecStart=/home/ubuntu/dotnet_arm64/dotnet /home/ubuntu/server/IoT_RaspberryServer.dll
Restart=always
# Restart service after 10 seconds if the dotnet service crashes:
RestartSec=10
KillSignal=SIGINT
SyslogIdentifier=iot_server
User=ubuntu
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=DOTNET_PRINT_TELEMETRY_MESSAGE=false

[Install]
WantedBy=multi-user.target
```
- Enable service: `sudo systemctl enable iot_server.service`
- Start service: `sudo systemctl start iot_server.service`
- Check the status to be sure: `sudo systemctl status iot_server.service`
