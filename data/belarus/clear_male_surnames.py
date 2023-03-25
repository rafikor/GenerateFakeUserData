# -*- coding: utf-8 -*-
"""
Created on Thu Mar 23 12:49:40 2023

@author: user
"""

ifile = open('male surnames_initial.txt','r',encoding='utf-8')
textlines = ifile.readlines()
ifile.close()

resultdata=[]
for line in textlines:
    if line.strip()=='':
        continue
    words=line.split(' ');
    if len(words)>0:
        if not words[0].endswith(','):
            continue
    
    
    ifSentence = True
    for word in words:
        print(word)
        if word.capitalize()[0]!=word[0]:
            ifSentence = False
            print(word)
            break
    if ifSentence:
        for word in words:
            resultdata.append(word.replace(',','').replace('.','').replace('\n',''))
        
ofile = open('male surnames_auto.txt','w')
for lastname in resultdata:
    ofile.write(lastname+'\n')
ofile.close()