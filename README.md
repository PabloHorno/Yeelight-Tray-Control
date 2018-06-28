# Yeelight Control in tray (Notification Panel)

A simple Yeelight control with which you can turn off, on and dim a Yeelight from the notification panel in windows.

![on](https://imgur.com/sgJn25V.png)
![off](https://imgur.com/Ao5Tm06.png)
![dim](https://imgur.com/MQ8nCnj.png)

## Getting Started

For this first version only one Yeelight can be controlled, and the local IP must be entered by hand in the constructor of the Yeelight class.

### Prerequisites

Visual Studio
Windows Forms


### Installing

Download the repository and open de .sln with Visual Studio

First
```
Open Yeelight.sln
```

In solution Explorer go to Form1.cs and in line 18 change the IP with your own Yeelight IP.
```
        Yeelight yeelight = new Yeelight("192.168.1.4");
```

Build and go.

## Authors

* **Pablo Horno** - *Initial work* - [PabloHorno](https://github.com/PabloHorno)

See also the list of [contributors](https://github.com/PabloHorno/Yeelight-Tray-Control/graphs/contributors) who participated in this project.

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details

## Acknowledgments

* To thoes one who will download and try my program ;)