#  set-executionpolicy remotesigned

$beoutRoot = 1;

while (0 -eq 0){
    #�ֶ�����
    $giturl_base="/GongAnSecurityCharts/SecurityChartsServer.git"
     #�ֶ�����
    $originBranch="master"

    $localBranch="master"
    $beout=$beoutRoot 
      $giturl="";
    Write-Host ""
    Write-Host ""
    Write-Host "--------------------------"
    Write-Host "����ǰ���̡�" + $giturl_base
    Write-Host "��Զ��ʹ�÷�֧��" + $originBranch
    Write-Host "�����ط�֧��" + $localBranch

    if($beout -eq 0){
     Write-Host "���Ƿ�ʹ�ö�̬IP����"
    } else {
     Write-Host "���Ƿ�ʹ�ö�̬IP����"
    }
   
     Write-Host "--------------------------"
    Write-Host '>> ������ʾ:  1 [�޸�origin��ַ], pull [��ȡ], push [����], pushall [�Զ��ύ������], exit [ȡ��], 0 [�����Ļ], sethome [���þ�����], setpub [���ù���]'

     Write-Host "����������:" -NoNewline
    $commond = Read-Host

    Write-Host "���������:" + $commond
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
       Write-Host ">>> �ѳɹ�����Origin��ַ: " + $giturl
    }
    if($commond -eq "setpub") {
      $beoutRoot = 1
    }
    if($commond -eq "exit") {
        Write-Host "�˳�����."
        Exit;
    }

    if(($beout -eq 1) -or ($commond -eq "1")) {
        $a=Invoke-WebRequest -Uri "http://www.wangziwen.vip:9200/GetIP" -UseBasicParsing
        $ip=$a.Content
        $giturl = "http://" + $ip + ":15002" + $giturl_base

        git remote set-url origin $giturl

        if($commond -eq "1") {
            Write-Host ">>> �ѳɹ�����Origin��ַ: " + $giturl
            continue;
        }

    } else {
        $giturl = "http://192.168.0.100:8570" + $giturl_base
    }


    if($commond -eq "pull") {
        Write-Host "������ȡ:"
        Write-Host "git.exe pull $giturl $originBranch"
        git.exe pull $giturl $originBranch
    } 

    if($commond -eq "push") {
        Write-Host "��������:"
        Write-Host "    git.exe push $giturl $localBranch":"$originBranch"
        git.exe push $giturl $localBranch":"$originBranch
    }

    if($commond -eq "pushall") {
         Write-Host "�������ύע�� (exit �˳��ύ) [Ĭ�� update]:" -NoNewline
         $commitdesc = Read-Host

         if($commitdesc -eq "exit") {
             continue;
         }

         if($commitdesc == "") {
            $commitdesc = "update"
         }

         Write-Host "�����Զ��ύ:"

         Write-Host "git add *"
         git add *

         Write-Host "git commit -m "$commitdesc""
         git commit -m "$commitdesc"

         Write-Host "    git.exe push $giturl $localBranch":"$originBranch"
         git.exe push $giturl $localBranch":"$originBranch
    }
}
