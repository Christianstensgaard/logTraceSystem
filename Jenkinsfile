pipeline {
    agent any

    triggers {
        pollSCM('* * * * *')
    }

    stages {
        stage("Build") {
            steps {
                echo 'Building solution'

                echo 'Starting Build MsgC'
                echo 'Starting the build of MonitoringAPI'

                sh 'dotnet publish -c Release -o ./out'
                sh 'cd ../../'

                echo 'Checking some git stuff'
                sh 'git status'
            }
        }
        stage("Test") {
            steps{
                echo 'doing some test'
            }
        }

        stage("Deploy") {
            steps{
                echo 'deplaying to Target: xxxxxxxxxx'
            }
        }
    }
}
