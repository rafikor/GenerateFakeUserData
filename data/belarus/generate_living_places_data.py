# -*- coding: utf-8 -*-
"""
Created on Thu Mar 23 12:49:40 2023

@author: user
"""

def writeData(filenameCSV,listToCSV):
    ofile = open(filenameCSV,'w',encoding='utf-8')
    for dataLivingPlace in listToCSV:
        stringToWrite=''
        for datapart in dataLivingPlace:
            stringToWrite+=str(datapart)+';'
        stringToWrite=stringToWrite[0:len(stringToWrite)-1]
        ofile.write(stringToWrite+'\n')
    ofile.close()

ifile = open('cities_and_villages_belarus.txt','r',encoding='utf-8')
textlines = ifile.readlines()

livingPlacesAbbreviations={'р.пасёлак': 'р.п.',
'рзд': 'рзд',
'рабочы пасёлак': 'р.п.',
"раз'езд": 'рзд',
'курортны пасёлак': 'к.п.',
'горад': 'г.',
'пасёлак ільнозавод': 'п.',
'хутар': 'х.',
'гарадскі пасёлак': 'г.п.',
'вёска': 'в.',
'к.пасёлак': 'к.п',
'станцыя':'с.',
'пасёлак':'п.',
'агорад':'а.г.',
'с.':'с.',
'п.':'п.'
}

dataCities=[]
dataVillages=[]
villageSovets = set()
placesTypes=set()
for line in textlines:
    line=line.strip()
    liveSoviet=line.split(',')
    livingPlace=liveSoviet[0]
    if len(liveSoviet)>1:
        villageSovets.add(liveSoviet[1].strip().replace('сельсавет','с/с').replace('гарадскі савет','г.с.'))
    livingPlaceTypeAndName=livingPlace.split(' ')
    isCity=livingPlaceTypeAndName[0]=='горад'
    
    livingPlaceType = ''
    livingPlaceName = ''
    for word in livingPlaceTypeAndName:
        if word.capitalize()[0]==word[0]:
            livingPlaceName+=word+' '
        else:
            livingPlaceType+=word+' '
    livingPlaceType = livingPlaceType.strip()
    livingPlaceName = livingPlaceName.strip()
    placesTypes.add(livingPlaceType)
    for key in livingPlacesAbbreviations.keys():
        livingPlaceType=livingPlaceType.replace(key,livingPlacesAbbreviations[key])    
    
    dataLivingPlace=[livingPlaceType+' '+livingPlaceName]
    if isCity:
        dataCities.append(dataLivingPlace)
    else:
        dataVillages.append(dataLivingPlace)

writeData('cities.csv',dataCities)
writeData('villages.csv',dataVillages)

ofile = open('living places types.txt','w',encoding='utf-8')
for typePlace in placesTypes:
    ofile.write(typePlace+'\n')
ofile.close()

ofile = open('villageSoviets.csv','w',encoding='utf-8')
for villageSovet in villageSovets:
    ofile.write(villageSovet+'\n')
ofile.close()