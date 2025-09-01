# Open OneDrive

This is an open DotNet library to use OneDrive api.

## Features
- Easy to use
- Fast and lightweight
- Cross-platform

## Samples

Get items in root folder
```csharp
var client = new OneDriveClient("accessToken");
var items = await client.GetItemsAsync("");
```

Get sub-folders in root folder
```csharp
var client = new OneDriveClient("accessToken");
var items = await client.GetItemsAsync("", filter: "folder ne null");
```