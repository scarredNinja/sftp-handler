# Assessment

This repo covers how I would approach creating an SFTP Handler to process a valid Dispatch Request, form the file to be sent to the 3PL and actioning the upload.

## Trigger

I choose to create the handler as an Azure function which could then be triggered by an HTTP request.

Considerations:
-> Could have used a Logic App to handle the SFTP connection but I didnt go that way as I wanted to have control over the design and implemntation of how I got the Request Data and then sent it. Discussion could be had on the pros and cons and a possible different design path could be used.

## Creating the File

The inbound request will have the Dispatch Request ID which will be used to query the database and get all the information requried to send to the 3PL

Based on the file format, it will then be created and stored in blob storage to be retained incase it needs to be resent.

## Uploading to the SFTP

When the file is successfully created, the upload process will commence.

I would store the Public Key for authentication in an Azure Key Store which I could then access to then form the connection string to the SFTP.

The connection information will be stored outside the function app and retrieved via settings which can be updated in Azure.

Once the connection is establised, the file will then be uploaded to the specified file.

### Missed/Considerations

Due to time constraints I missed on implementing the SFTP upload fully or to fully test it works.

I would extend it to add Public Key Authentication through Azure Key Store. I would also have SFTP values passed in via Azure or stored externally to avoid hardcoding values.

Some of the helpers/services separated could be put in one class but separated to use Single Responsiblity principal and is easier to test. This can have its downsides but since the dependancies are small it makes sense.

Function App not fully tested so may break on load