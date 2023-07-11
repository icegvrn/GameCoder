function CreateAliens()
  local aliens = {}
  aliens.nombre = 0
    aliens.AjouteAlien = function(n)
      aliens.nombre = aliens.nombre + n
      print("Nombre d'aliens est égal à ",aliens.nombre)
    end
  return aliens
end