@ECHO OFF 
@TITLE ����WINDOWS����
ECHO==============================================================
ECHO=
ECHO         WINDOWS�����������
ECHO=
ECHO==============================================================
@ECHO OFF
@SET serviceName=
@NET STOP %serviceName%  
@NET START %serviceName%  
PAUSE