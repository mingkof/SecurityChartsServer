Scaffolding has generated all the files and added the required dependencies.

However the Application's Startup code may required additional changes for things to work end to end.
Add the following code to the Configure method in your Application's Startup class if not already done:

        app.UseMvc(routes =>
        {
          route.MapRoute(
            name : "areas",
            template : "{area:exists}/{controller=Home}/{action=Index}/{id?}"
          );
        });


md $(ProjectDir)bin\Release\PublishOutput
md $(ProjectDir)bin\Release\PublishOutput\Node
md $(ProjectDir)bin\Release\PublishOutput\node_modules

xcopy $(ProjectDir)Node  $(ProjectDir)bin\Release\PublishOutput\Node /e/h/y
xcopy $(ProjectDir)..\..\部署\公安\node_modules  $(ProjectDir)bin\Release\PublishOutput\node_modules /e/h/y
