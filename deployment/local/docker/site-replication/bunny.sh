# docker login registry.tanzu.vmware.com -u $HARBOR_USER -p $HARBOR_PASSWORD
#docker pull registry.tanzu.vmware.com/rabbitmq/vmware-tanzu-rabbitmq:1.2.3

#-h bunny --network bridge
docker run -it --rm -h bunny --network edge --name bunny -v "$PWD/rabbitmq_enabled_plugins:/etc/rabbitmq/enabled_plugins" -p 5672:5672 -p 5552:5552 -p 15672:15672  -p  1883:1883 rabbitmq:3.10.10-management
