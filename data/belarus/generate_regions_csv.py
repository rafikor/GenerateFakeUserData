# -*- coding: utf-8 -*-
"""
Created on Thu Mar 23 12:49:40 2023

@author: user
"""


ifile = open('regions.txt','r',encoding='utf-8')
textlines = ifile.readlines()

data=[]
for line in textlines:
    line=line.strip()
    if len(line)!=0:
        data.append(line.split('\t')[0])
        
ofile = open('regions.csv','w')
for region in data:
    region = region.replace('раён','р-н')
    ofile.write(region+'\n')
ofile.close()