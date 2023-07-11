require("aliens")
local heros = {}

local boss = {}
boss.energie = 1000
boss.maxEnergie = 1000

-- Identité de mon héros
heros.nom = "John Doe"
heros.classe = "Archer"
heros.race = "Humain"
heros.energie = 100
heros.maxEnergie = 80
print("Energie",heros.energie)
heros.pointsDeVie = 20
heros.pointsDeMagie = 20
heros.protectionArmure = 10
heros.bouclierEquipe = true

-- Inventaire de mon héros
heros.inventaire = {}
heros.inventaire.arme = "Epée longue"
heros.inventaire.compagnon = "Chien des enfers"
heros.inventaire.armure = "Cotte de maille 10"
heros.inventaire.potions = {}

-- Détails des potions dans l'inventaire
heros.inventaire.potions[0] = "Potion de soin"
heros.inventaire.potions[1] = "Potion de soin améliorée"
heros.inventaire.potions[2] = "Potion de concentration"
heros.inventaire.potions[3] = "Potion d'aveuglement"

-- Test de mon héros 
print("Héros sélectionné : "..heros.nom.. " de la race ".. heros.race..  " et de la classe "..heros.classe..". Il a "..heros.pointsDeVie.. " points de vie. Son inventaire indique qu'il porte une "..heros.inventaire.arme.. " comme arme et contient les potions : "..heros.inventaire.potions[0]..", "..heros.inventaire.potions[1]..", "..heros.inventaire.potions[2]..", "..heros.inventaire.potions[3])

function Touche(personnage,nbPoints) 
    personnage.energie = personnage.energie - nbPoints
    print("Héros touché ! Il a désormais un nombre d'énergie de : ",personnage.energie)
  end
  
function RestoreEnergie(personnage) 
    personnage.energie = personnage.maxEnergie
    print("Maintenant le personnage a pour énergie :",personnage.energie)
  end
  
  Touche(heros,20)
  Touche(boss,100)
  RestoreEnergie(heros)
  RestoreEnergie(boss)
  
  if heros.energie == 100 
    then Touche(heros,20) 
    else Touche(boss,20) 
  end
  
  if heros.inventaire.arme ~= "Jack" then print("OULA") end

  for compteur = 1, 10 
    do print(compteur) 
  end
  
  
  compteur = 0
  
  while compteur < 10 do 
    compteur = compteur + 1
    print("Compteur : "..compteur)
  end
  
  
  for compteur = 1, 100 
  do 
    if compteur == 50 
      then print("Je suis à la moitié avec "..compteur) 
    end
  end
  
  for colonne = 1, 40 
  do 
    if colonne == 40 then
    for ligne = 1, 10 
      do
        print("Je parcours sur colonne "..colonne.." la ligne "..ligne)
      end
    end
  end
  
  heros.x = 100
  print(heros["x"])

 print(heros.inventaire.potions[0])
 print(#heros.inventaire)
 
 for n, c in ipairs(heros.inventaire.potions) do
 print(c)
end


mesAliens = CreateAliens()

mesAliens.AjouteAlien(1)
mesAliens.AjouteAlien(4)
mesAliens.AjouteAlien(20)