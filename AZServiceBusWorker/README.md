# Azure Service Bus Example
This is a POC application to use Azure Service Bus in c# application

## Environment Variables
1. **Producer**: true when you want to start service as a Producer of messages
2. **Consumer**: true when you want to start service as a Consuemr of messages
3. **ServiceBusConnectionString**: the primary connection string of the azure service bus 
4. **QueueName**: the queue name of the service bus you want to send/receice messages


## Steps to run
start docker in you machine
### Build image
```bash
docker build -t azservicebus-worker:v1 .
```
### Start Producer
```bash
docker run -it -e Producer='true' \ 
    -e ServiceBusConnectionString="<connection-string>" \
    -e QueueName="<queue-name>" \
    -h producer-a \
    azservicebus-worker:v1
```

### Start Consumer
```bash
docker run -e Consumer='true' \
    -e ServiceBusConnectionString="<connection-string>" \
    -e QueueName="<queue-name>" \
    -h consumer-a \
    azservicebus-worker:v1
```
