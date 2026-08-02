# Markadian Playlister (1.0.4)

## How to install
- Download the .exe file from the assets section
- Place it in your desired folder
### Your first run
- Running for the first time the application will detect if there any **dependencies** missing. If they are they will **download automatically**.
- **A Resources folder will be created**. Don't delete that folder, otherwise the application will have to download those dependencies again.
- The depencies will take around **100MB** so make sure you have enough space.
- A settings file will be created in .json format in a new or already existing Preferences folder. **Don't edit it manually**. You can use the preferences menu inside the application by accessing Edit -> Preferences. Always click apply after you are done with changing the settings
- If any file is corrupted it will be downloaded again and replaced.
- Video playback (from 1.0.4 onwards) is disabled by default to save size. You need to enable the setting in preferences and they will be downloaded automatically (requires extra 135mb of space). 


## Main Features

### Song Download Functionality
- User can copy & paste a youtube URL to download a song
- User can adjust his preferences in the designated preferences window
- URL complex parsing regardless if you copy a playlist or song
- User can add multiple songs to the queue if it is enabled
- User can select the desired bitrate
- User can select the path to download from
- User has a deisgnated panel where he can search for songs on youtube automatically
- User can adjust the search count in the preferences window
- User can select the song displayed as a card in the result section and download the desired song
- User can click on multiple results to download multiple songs at once
- User can preview the video by right clicking on a card after a search.
- User can choose to list only .mp3 or both .mp3 and .mp4 files
- User can download .mp4 files as well with chosen quality.

### Metadata and Organization Functionality

- User can drag and drop songs into the list or metadata panel editor (if enabled)
- User can click on any shown file in the list view which points to the download path if the user wants to edit a specific metadata's file
- User can see the files in the chosen downloaded folder only which confers easy access to editing those files
#### User can edit multiple metadata features which include the following:

- Title
- Contributing Artist
- Album
- Genre
- Year
- BPM
- Disc Number
- Key
- Cover Image(s)
##### Note: More flexibility to be added later


### System Functionalities

- Light and Dark theme
- Automatic downloading for the required resources (requires internet connection)
- Reindexing the files in the menu toolbar
- Customisation of panel views
- Customisable Settings in the preferences menu
- Recreation of the .json configuration files in case they are lost
- Customisable resource directory
- Supported file formats are .wav and .mp3

### Warning Notes

- This app requires certain dependencies which have to be downloaded without them it will break certain functionalities
- This app is intended for personal use
- Do not change the names of the resources files or folders. The application is specifically looking for those names
- VLC dependencies are also available for direct download
- Third party resources depend also on servers from github.
