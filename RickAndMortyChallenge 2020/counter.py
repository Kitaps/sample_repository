def letter_appearance(letter, json_data):
    # Recives the letter to look for and the result data, parsed as json. 
    # Regardless of the data type, we always look for the letter under the key "name".
    # Returns the counted number as int.
    
    # Therefore, for each entry in the data, we count the letter ocurrences in the entrys name. 
    # We set the entry name in lowercase to make it case insensitive. 
    name_counter_iterator = map(lambda entry: entry["name"].lower().count(letter), json_data)
    # Later we return the appearence number.
    return sum(name_counter_iterator)

if __name__ == "__main__":
    pass
    # from api_collector import api_get
    # print(letter_appearance("l", api_get("location")))
    # print(letter_appearance("c", api_get("character")))
    # print(letter_appearance("e", api_get("episode")))