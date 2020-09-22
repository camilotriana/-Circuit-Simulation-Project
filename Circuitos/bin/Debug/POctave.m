clc
clear
pkg load control
s=tf('s');
R=100;
L=0.01;
C=0.2;
Z=3;
ZL=2;
V=1;
Z1=L*s+ZL;
Z2=Z1/(1+C*s*Z1);
ZT=Z+Z2+R;
I=V/ZT;
VZ2=I*Z2;
IZ1=VZ2/Z1;
IZL=IZ1;
VZL=IZ1*ZL;
GVZL=VZL/V;
GIZL=IZL/I;
PZL=VZL*IZL;
P=V*I;
GPZL=PZL/P;
[v,t1]=step(GVZL);
c=length(t1);
tiempo=t1(c)*1.1;
[v,t1]=step(GVZL,tiempo,tiempo/1000);
dlmwrite('D:\Mis documentos\Escritorio\Circuitos Final\Circuitos\bin\Debug\t1.txt',t1,'\n');
dlmwrite('D:\Mis documentos\Escritorio\Circuitos Final\Circuitos\bin\Debug\v.txt',v,'\n');
[i,t1]=step(GIZL);
c=length(t1);
tiempo=t1(c)*1.1;
[i,t1]=step(GIZL,tiempo,tiempo/1000);
dlmwrite('D:\Mis documentos\Escritorio\Circuitos Final\Circuitos\bin\Debug\t1.txt',t1,'\n');
dlmwrite('D:\Mis documentos\Escritorio\Circuitos Final\Circuitos\bin\Debug\i.txt',i,'\n');
