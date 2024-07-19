FROM unityci/editor:2020.1.17f1-webgl-0.12.0

WORKDIR /app

# Запустите команду для генерации запроса активации
RUN /opt/unity/Editor/Unity -batchmode -nographics -logFile /dev/stdout -createManualActivationFile