## Use an official OpenJDK runtime as a parent image
#FROM openjdk:17-jdk-alpine
#
## Set the working directory in the container
#WORKDIR /app
#
## Copy the packaged jar file into the container
#COPY target/ClothingStoreManager-0.0.1-SNAPSHOT.jar app.jar
#
## Expose the port that the application will run on
#EXPOSE 4040
#
## Set environment variables for database connection
#ENV DB_PORT=3306
#ENV DB_HOST=mysql.railway.internal
#ENV DB_NAME=railway
#ENV DB_USERNAME=root
#ENV DB_PASSWORD=fKQaHOWvHVUKejfvomHRxibNbtcbDzXR
#
## Run the jar file
#ENTRYPOINT ["java", "-jar", "app.jar"]



# docker file for railway deployment
FROM openjdk:17
ADD ./ClothingStoreManager-0.0.1-SNAPSHOT.jar ClothingStoreManager-0.0.1-SNAPSHOT.jar
ENTRYPOINT ["java","-jar","ClothingStoreManager-0.0.1-SNAPSHOT.jar"]
