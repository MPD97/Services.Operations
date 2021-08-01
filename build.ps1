docker build -t service.operations . ;
docker tag service.operations mateusz9090/operations:local ;
docker push mateusz9090/operations:local