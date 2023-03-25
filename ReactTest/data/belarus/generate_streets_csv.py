# -*- coding: utf-8 -*-
"""
Created on Thu Mar 23 12:49:40 2023

@author: user
"""

types={'кальцо':'к.', 'дарога':'д.', 'праспект':'п.', 'праезд':'п.', 'плошча':'п.', 
       'завулак':'з.', 'тракт':'т.', 'тупік':'т.', 'спуск':'с.', 'бульвар':'б.', 
       'вуліца':'в.', 'набярэжная':'н.', 'шаша':'ш.'}

data=[]

regionsWithStreets=['brest','barysaw','vileyka','minsk']
for regionName in regionsWithStreets:
    ifile = open(regionName+'_streets.txt','r',encoding='utf-8')
    textlines = ifile.readlines()
    

    typestreets=set();
    for line in textlines:
        line=line.strip()
        if len(line)!=0:
            street=line.split('\t')[0].strip()
            for key in types.keys():
                if key in street:
                    street = types[key]+' '+street.replace(key,'').strip()
            data.append(street)
            streetSplitted=street.split(' ')
            print(streetSplitted)
            typestreets.add(streetSplitted[len(streetSplitted)-1])
        
ofile = open('streets.csv','w',encoding='utf-8')

for street in data:
    #region = region.replace('раён','р-н')
    ofile.write(street+'\n')
ofile.close()
print(typestreets)