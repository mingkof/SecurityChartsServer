const psshell = require('node-powershell');

export default class PSRunner {

    Run(commandList:Array<string>, callback:RunnerCallbackType) {
          commandList.forEach(function(cmd,index,array){
            
            let ps = new psshell({
              executionPolicy: 'Bypass',
              noProfile: true
            });

            ps.addCommand(cmd)

            ps.invoke()
            .then(output => {
              if(callback != null) {
                callback(index,output);
              }
              // console.log(output);
            })
            .catch(err => {
              console.log(err);
              ps.dispose();
            });

          });
     
    }

}
