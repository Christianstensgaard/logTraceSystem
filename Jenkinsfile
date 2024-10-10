pipeline {
    agent any

    triggers {
        pollSCM('* * * * *')
    }

    stages {
        stage("Build") {
            steps {
                echo 'Hello World'
                echo 'Building dependencies'

                //Building externals libiays
                echo 'Starting Build MsgC'
                sh 'cd Lib/MsgC'
                sh 'dotnet publish -c Release -o ./out'
                sh 'cd ../../'
                //Building Monitoring System
                echo 'Starting the build of MonitoringAPI'
                

            }
        }
    }
}
