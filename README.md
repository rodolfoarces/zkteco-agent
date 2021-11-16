# Agent

Biometric device terminal connection agent

## Getting started

This is a POC (proof of concept) connection agent for Biometric terminal devices.

## Compilation

Using Microsoft Visual Studio Community 2019 (Version 16.11.5)
Packaged (Nuget): [Commandline](https://github.com/commandlineparser/commandline), [log4net](http://logging.apache.org/log4net/) and [zkemkeeper](devices/README.md)

## Usage

The executable is used with connection parameters or default parameters will be set:

```
zkteco-cli.exe [-h|--host FQDN_or_IP] [-p|--port port_number] [-P|--password device_password]
```

## Diagnostics

In the executable folder a file "zkteco-cli.log" is created with debug information.

## Licenses

This program is free software, licensed under the terms of the [GPL v.3](LICENSE)

Some components of the software such as libraries, SDKs and other have their own license and are referenced in their respective comments, download links, etc. if available.
