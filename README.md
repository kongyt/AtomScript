# AtomScript
a scripting language for c#

# Example
class A{  
　　func PrintA(){  
　　　　print "A";  
　　}  
}  
  
class B : A{  
　　func PrintB(a, b, c){  
　　　　print "B";  
　　}  
}  
  
var a = B();  
var num = 1;  
num = num + 1;  
print num;  
  
for (var i = 0; i < 10; i = i + 1){  
　　print i;  
}  
  
while(true){  
　　print true;  
}  
  
num1 = 1;  
num2 = 2;  
if(num1 < num2){  
　　print num2;}  
else{  
　　print num1;  
}  