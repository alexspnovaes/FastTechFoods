@echo off

echo ===============================
echo CONFIGURANDO DOCKER DO MINIKUBE
echo ===============================
FOR /F "tokens=*" %%i IN ('minikube docker-env --shell cmd') DO %%i

echo ===============================
echo BUILDANDO IMAGENS DO PROJETO
echo ===============================
docker build -t authservice:latest -f src/AuthService/Dockerfile .
docker build -t orderservice:latest -f src/OrderService/Dockerfile .
docker build -t kitchenservice:latest -f src/KitchenService/Dockerfile .
docker build -t menuservice:latest -f src/MenuService/Dockerfile .

echo ===============================
echo APLICANDO YAMLs NO KUBERNETES
echo ===============================
kubectl apply -f k8s/auth-service/deployment.yaml
kubectl apply -f k8s/order-service/deployment.yaml
kubectl apply -f k8s/kitchen-service/deployment.yaml
kubectl apply -f k8s/menu-service/deployment.yaml

echo ===============================
echo REINICIANDO DEPLOYMENTS
echo ===============================
kubectl rollout restart deployment authservice
kubectl rollout restart deployment orderservice
kubectl rollout restart deployment kitchenservice
kubectl rollout restart deployment menuservice

echo ===============================
echo DEPLOY COMPLETO 🚀
echo ===============================
kubectl get pods -A
