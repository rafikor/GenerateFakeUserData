# -*- coding: utf-8 -*-
"""
Created on Thu Mar 23 12:49:40 2023

@author: user
"""


villages=[]
gminas=[]
powiats=[]


ifile = open('villages.htm','r',encoding='utf-8')
text = ifile.read()
start=0

while True:
    start=text.find('wies_',start)
    if start<0:
        break
    start=text.find("'>",start)+2
    end=text.find("<",start)
    village=text[start:end].replace('Wieś ','')
    villages.append(village)
    
    start=text.find('gmina_',start)
    start=text.find("'>",start)+2
    end=text.find("<",start)
    gmina=text[start:end]
    gminas.append(gmina)
    
    start=text.find('powiat_',start)
    start=text.find("'>",start)+2
    end=text.find("<",start)
    powiat=text[start:end].replace('powiat ','')
    powiats.append(powiat)

    
ofileVillages = open('villages.csv','w',encoding='utf-8')
for village in villages:
    ofileVillages.write(village+'\n')
ofileVillages.close()

ofileGminas = open('gminas.csv','w',encoding='utf-8')
for gmina in gminas:
    ofileGminas.write(gmina+'\n')
ofileGminas.close()

ofilePowiats = open('powiats.csv','w',encoding='utf-8')
for powiat in powiats:
    ofilePowiats.write(powiat+'\n')
ofilePowiats.close()