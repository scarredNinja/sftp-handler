# sandford-assessment

This repo covers how I would approach creating an SFTP Handler to process a valid Dispatch Request, form the file to be sent to the 3PL and actioning the upload.

## Trigger

I choose to create the handler as an Azure function which could then be triggered by an HTTP request.

Considerations:
-> Could have used a Logic App to handle the SFTP connection but I didnt go that way as I wanted to have control over the design and implemntation of how I got the Request Data and then sent it. Discussion could be had on the pros and cons and a possible different design path could be used.

## Creating the File


## Uploading to the SFTP

I would store the Public Key for authentication in an Azure Key Store which I could then access to then form the connection string to the SFTP