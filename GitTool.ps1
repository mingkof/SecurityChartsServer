#  set-executionpolicy remotesigned

$beoutRoot = 1;

while (0 -eq 0){
    #手动设置
    $giturl_base="/GongAnSecurityCharts/SecurityChartsServer.git"
     #手动设置
    $originBranch="master"

    $localBranch="master"
    $beout=$beoutRoot 
      $giturl="";
    Write-Host ""
    Write-Host ""
    Write-Host "--------------------------"
    Write-Host "【当前工程】" + $giturl_base
    Write-Host "【远程使用分支】" + $originBranch
    Write-Host "【本地分支】" + $localBranch

    if($beout -eq 0){
     Write-Host "【是否使用动态IP】否"
    } else {
     Write-Host "【是否使用动态IP】是"
    }
   
     Write-Host "--------------------------"
    Write-Host '>> 命令提示:  1 [修改origin地址], pull [拉取], push [推送], pushall [自动提交并推送], exit [取消], 0 [清空屏幕], sethome [设置局域网], setpub [设置公网]'

     Write-Host "请输入命令:" -NoNewline
    $commond = Read-Host

    Write-Host "您输入的是:" + $commond
    Write-Host ""

    if($commond -eq "1") {
       $beout = 1;
    }
    if($commond -eq "0") {
       cls
       continue
    }
    if($commond -eq "sethome") {
       $beoutRoot = 0
       $giturl = "http://192.168.0.100:8570" + $giturl_base
       git remote set-url origin $giturl
       Write-Host ">>> 已成功更新Origin地址: " + $giturl
    }
    if($commond -eq "setpub") {
      $beoutRoot = 1
    }
    if($commond -eq "exit") {
        Write-Host "退出命令."
        Exit;
    }

    if(($beout -eq 1) -or ($commond -eq "1")) {
        $a=Invoke-WebRequest -Uri "http://www.wangziwen.vip:9200/GetIP" -UseBasicParsing
        $ip=$a.Content
        $giturl = "http://" + $ip + ":15002" + $giturl_base

        git remote set-url origin $giturl

        if($commond -eq "1") {
            Write-Host ">>> 已成功更新Origin地址: " + $giturl
            continue;
        }

    } else {
        $giturl = "http://192.168.0.100:8570" + $giturl_base
    }


    if($commond -eq "pull") {
        Write-Host "正在拉取:"
        Write-Host "git.exe pull $giturl $originBranch"
        git.exe pull $giturl $originBranch
    } 

    if($commond -eq "push") {
        Write-Host "正在推送:"
        Write-Host "    git.exe push $giturl $localBranch":"$originBranch"
        git.exe push $giturl $localBranch":"$originBranch
    }

    if($commond -eq "pushall") {
         Write-Host "请输入提交注释 (exit 退出提交) [默认 update]:" -NoNewline
         $commitdesc = Read-Host

         if($commitdesc -eq "exit") {
             continue;
         }

         if($commitdesc == "") {
            $commitdesc = "update"
         }

         Write-Host "正在自动提交:"

         Write-Host "git add *"
         git add *

         Write-Host "git commit -m "$commitdesc""
         git commit -m "$commitdesc"

         Write-Host "    git.exe push $giturl $localBranch":"$originBranch"
         git.exe push $giturl $localBranch":"$originBranch
    }
}
