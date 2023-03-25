# -*- coding: utf-8 -*-
"""
Created on Thu Mar 23 12:49:40 2023

@author: user
"""

ifile = open('male names.txt','r',encoding='utf-8')
textlines = ifile.readlines()
ifile.close()
replacesEnd={'й':'еўна',
          'ь':'еўна',
          'ў':'ваўна',
          'а':'аўна',
          }

resultdata=[]
for line in textlines:
    line=line.strip()
    endsToReplase=replacesEnd.keys()
    isProcessed = False
    for endToReplase in endsToReplase:
        if line.endswith(endToReplase):
            line=line[0:len(line)-len(endToReplase)]+replacesEnd[endToReplase]
            isProcessed = True
            break
    if not isProcessed:
        line = line + 'аўна'
    resultdata.append(line)
        
ofile = open('female patronymics.txt','w')
for lastname in resultdata:
    ofile.write(lastname+'\n')
ofile.close()