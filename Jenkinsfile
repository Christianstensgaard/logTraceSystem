pipeline{
  agent any
  trigger{
    pollSCM("* * * * *")
    git(poll: true, url: '')
  }

  stages{
      stage("Build"){
        steps{
            step{
              echo(message: 'hello World')
            }
        }

      }
  }
}