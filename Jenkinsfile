pipeline {
    agent any

    triggers {
        pollSCM('* * * * *')  // Triggers the pipeline based on SCM polling
    }

    stages {
        stage("Build") {
            steps {
                echo 'Hello World'  // Directly place echo command inside steps


                echo 'Building dependensies'
                cd 'ls'
            }
        }
    }
}
