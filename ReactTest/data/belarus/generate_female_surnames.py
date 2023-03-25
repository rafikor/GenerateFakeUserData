# -*- coding: utf-8 -*-
"""
Created on Thu Mar 23 12:49:40 2023

@author: user
"""

ifile = open('male surnames.txt','r')
textlines = ifile.readlines()
ifile.close()
replacesEnd={'ы':'ая',
          'оў':'ова',
          'кі':'кая',
          'ін':'іна',
          'аў':'ава',
          'еў':'ева',
          'ёў':'ёва',
          'ын':'ына'
          }

resultdata=[]
for line in textlines:
    line=line.strip()
    endsToReplase=replacesEnd.keys()
    for endToReplase in endsToReplase:
        if line.endswith(endToReplase):
            line=line[0:len(line)-len(endToReplase)]+replacesEnd[endToReplase]
    resultdata.append(line)
        
ofile = open('female surnames.txt','w')
for lastname in resultdata:
    ofile.write(lastname+'\n')
ofile.close()