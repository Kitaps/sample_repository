# Module to get the API data and preprocess it
import requests
import numpy as np
from threading import Thread

def api_get(category):
    # Recives a category string: "location"; "character"; "episode"
    # Acess API and with GET query
    # We get the first page and information of how many pages to expect
    link = f"https://rickandmortyapi.com/api/{category}/"
    first_data = requests.get(link)

    # Make sure the request was made properly
    if first_data.status_code != 200:
        print(f"Unsuccessful request: {first_data.status_code}")
        return []

    # We want it parsed as JSON
    first_data = first_data.json()

    # We get the number of pages for the category to get
    pages = int(first_data["info"]["pages"])

    # We initialize an array to fill with the respective page results
    page_results = np.empty(pages, dtype=list)

    # We save the first page results, which have already been requested
    page_results[0] = first_data["results"]

    # Now the fun starts, we will make multiple threads, one for each page. 
    # These threads will save their results in their respective page_result slot. 
    threads = []
    for i in range(1, pages):
        # For each page, we make a thread to get the page data
        t = Thread(target=get_page, args=(link, i, page_results))
        # We start the thread and save it to be accesible
        t.start()
        threads.append(t)
    
    # We now make that the function waits for all threads to finish before continuing
    for t in threads:
        t.join()

    # Once all data has been get, we join the pages data into one
    results = np.concatenate(page_results)
    return results


def get_page(link, index, destiny):
    # Recives the category link, the page_results index of where to save the data
    # and the desteny of where to save (page_results)

    # Pages are always one more than the index
    # For example the first page has id 1, but is saved in page_results[0]
    page_id = index + 1
    # We take the category link and the index and concatenate it into the page link.
    page_link = "".join([link, "?page=", str(page_id)])

    page_data = requests.get(page_link).json()

    # We save the get data in the corresponding memory space
    destiny[index] = page_data["results"]


if __name__ == "__main__":
    pass
    
    # Parameters are: "location" ; "character" ; "episode"
    
    # data = api_get("episode")
    # print(data[0])
    # print()
    # print(data[1])
    # print()
    # print(data[-1])
    

