@ECHO OFF 
@TITLE ��װWINDOWS����
ECHO==============================================================
ECHO=
ECHO         WINDOWS�������װ
ECHO=
ECHO==============================================================
@ECHO OFF
@SET serviceName=
@SET targetEXE=
@SET serviceDescription=
@SC CREATE %serviceName% binPath= "%~dp0ServicePlugin.exe %targetEXE%" START= AUTO
@SC DESCRIPTION %serviceName% %serviceDescription%
@NET START %serviceName%  
PAUSE