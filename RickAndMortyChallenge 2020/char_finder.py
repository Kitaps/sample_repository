import numpy as np

# Module to find a episodes characters (and locations)

def chars_from(episode, characters):
    # Recives an episode JSON object 
    # Returns a tuple with the characer JSON
    
    # We only need the id from the link
    characters_ids = map(lambda link: id_from_character(link), episode["characters"])
    # Now we turn it into a tuple to be usable as index
    characters_ids = list(characters_ids)
    
    # Return the characters from the id_list
    return characters[characters_ids]

def id_from_character(link):
    # Recives and link string
    # Returns the id --> It's not actually the id but the index
    # Example: from "https://rickandmortyapi.com/api/character/2"  --> 1
    #          from "https://rickandmortyapi.com/api/character/41" --> 40
    # For this we asume that the first part of the link 
    # ("https://rickandmortyapi.com/api/character/") stays the same. 

    # We slice the string at the caracter 42, and return the id as int
    return int(link[42:]) - 1

def origins_from(chars):
    # Recive an character array
    # Return the origin location set and number of locations

    # Map: For each character in chars get the origin location name
    # Set(map) --> remove repeated locations
    locations = set(map(lambda char: char["origin"]["name"], chars))
    return locations


if __name__ == "__main__":
    print(id_from_character("https://rickandmortyapi.com/api/character/342"))