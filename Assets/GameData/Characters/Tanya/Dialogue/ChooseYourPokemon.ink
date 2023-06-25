-> main

=== main ===
Hey Henry! It's good to see you. How are you doing today?
    + [I'm doing great, thanks for asking.]
        -> chosen("Charmander")
    + [I've had better days, but I'm good.]
        -> chosen("Bulbasaur")
    + [It's been a tough day]
        -> chosen("Squirtle")
        
=== chosen(pokemon) ===
You choose {pokemon}!
-> END