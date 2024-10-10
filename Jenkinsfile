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
                sh 'ls'
        }
    }
}
