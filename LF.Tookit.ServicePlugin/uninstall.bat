@ECHO OFF 
@TITLE ж��WINDOWS����
ECHO==============================================================
ECHO=
ECHO          WINDOWS����ж��
ECHO=
ECHO==============================================================
@ECHO OFF 
@SET serviceName=
@NET STOP %serviceName%  
@SC DELETE %serviceName% 
PAUSE