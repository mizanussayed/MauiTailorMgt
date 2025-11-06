# Bluetooth Thermal Printer Integration - Setup Guide

## Overview
Your MYPM app now supports printing QR codes via ESC/POS Bluetooth thermal printers.

## Features Added
1. ? Bluetooth device discovery and pairing
2. ? ESC/POS printer command support
3. ? Image printing (QR codes)
4. ? Text printing capability
5. ? User-friendly device selection dialog

## Files Modified/Created

### New Files:
- `MYPM/Services/IBluetoothPrinterService.cs` - Service interface
- `MYPM/Services/BluetoothPrinterService.cs` - Bluetooth printer implementation
- `MYPM/Docs/BLUETOOTH_PRINTER_SETUP.md` - This guide

### Modified Files:
- `MYPM/MYPM.csproj` - Added Plugin.BLE and ESCPOS_NET packages
- `MYPM/MauiProgram.cs` - Registered BluetoothPrinterService
- `MYPM/Pages/Views/ShareQR.xaml` - Added Print and Close buttons
- `MYPM/Pages/Views/ShareQR.xaml.cs` - Added print functionality
- `MYPM/Pages/OrderDetailsPage.xaml.cs` - Injected printer service
- `MYPM/Platforms/Android/AndroidManifest.xml` - Added Bluetooth permissions

## How to Use

### 1. Pair Your Printer
Before using the app:
1. Open Android Settings
2. Go to Bluetooth settings
3. Pair your ESC/POS thermal printer
4. Note the printer name (e.g., "BlueTooth Printer", "RPP02N", etc.)

### 2. Print a QR Code
1. Open an order in the app
2. Tap the "Share" button
3. A popup will appear with the QR code
4. Tap **"Print via Bluetooth"**
5. Select your paired printer from the list
6. Wait for the connection confirmation
7. The QR code will print automatically!

### 3. Common Printer Names
- BlueTooth Printer
- RPP02N
- POS-58
- MTP-II
- Inner Printer

## Permissions Required

The following Android permissions are now included:
- `BLUETOOTH` - Basic Bluetooth access
- `BLUETOOTH_ADMIN` - Bluetooth device management
- `BLUETOOTH_SCAN` - Scan for Bluetooth devices (Android 12+)
- `BLUETOOTH_CONNECT` - Connect to Bluetooth devices (Android 12+)
- `ACCESS_FINE_LOCATION` - Required for Bluetooth scanning
- `ACCESS_COARSE_LOCATION` - Alternative location permission

## Troubleshooting

### "No paired Bluetooth devices found"
**Solution:** Pair your printer in Android Bluetooth settings first.

### "Failed to connect to printer"
**Possible causes:**
- Printer is turned off
- Printer is out of range
- Printer is already connected to another device
- Battery is low

**Solution:** 
1. Ensure printer is powered on
2. Move closer to the printer
3. Turn off Bluetooth, wait 5 seconds, turn it back on
4. Restart the printer

### "Failed to print"
**Possible causes:**
- Printer ran out of paper
- Weak Bluetooth connection
- Printer busy with another job

**Solution:**
1. Check if paper is loaded
2. Move closer to the printer
3. Try printing again

### Permission Denied
**Solution:** 
1. Go to App Settings ? Permissions
2. Enable all Bluetooth and Location permissions
3. Restart the app

## Supported Printer Models

This implementation works with most ESC/POS compatible thermal printers:
- 58mm thermal printers
- 80mm thermal printers
- Bluetooth-enabled receipt printers

## Technical Details

### Image Specifications
- **Format:** PNG (converted from Visual Element)
- **Max Width:** 384 dots (for 80mm paper)
- **Alignment:** Center
- **Quality:** 1-bit monochrome (thermal printer compatible)

### ESC/POS Commands Used
- Initialize printer
- Center alignment
- Print image
- Feed lines (3 lines)
- Full paper cut

### Bluetooth Connection
- Uses Plugin.BLE for cross-platform Bluetooth support
- Supports BLE (Bluetooth Low Energy) and Classic Bluetooth
- Auto-discovery of paired devices
- Manual scanning for unpaired devices

## Future Enhancements

Possible improvements for future versions:
- [ ] Remember last used printer
- [ ] Print preview
- [ ] Adjust image size/quality
- [ ] Print multiple copies
- [ ] Custom paper size selection
- [ ] Print order details along with QR code

## Package Information

### Plugin.BLE (v3.1.0)
- Cross-platform Bluetooth LE plugin
- Android 5.0+ (API 21+)
- License: Apache 2.0

### ESCPOS_NET (v3.0.0)
- ESC/POS printer command library
- Supports EPSON, Star, and compatible printers
- License: MIT

## Support

For issues or questions:
1. Check Android Bluetooth settings
2. Verify printer compatibility
3. Check app permissions
4. Review error messages in the app

---

**Note:** This feature is currently Android-only. iOS support would require different Bluetooth handling due to iOS restrictions.
