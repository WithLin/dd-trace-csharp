#----------------------------------------------------------------
# Generated CMake target import file for configuration "Debug".
#----------------------------------------------------------------

# Commands may need to know the format version.
set(CMAKE_IMPORT_FILE_VERSION 1)

# Import target "glog::glog" for configuration "Debug"
set_property(TARGET glog::glog APPEND PROPERTY IMPORTED_CONFIGURATIONS DEBUG)
set_target_properties(glog::glog PROPERTIES
  IMPORTED_LINK_INTERFACE_LANGUAGES_DEBUG "CXX"
  IMPORTED_LOCATION_DEBUG "${_IMPORT_PREFIX}/debug/lib/glog.lib"
  )

list(APPEND _IMPORT_CHECK_TARGETS glog::glog )
list(APPEND _IMPORT_CHECK_FILES_FOR_glog::glog "${_IMPORT_PREFIX}/debug/lib/glog.lib" )

# Commands beyond this point should not need to know the version.
set(CMAKE_IMPORT_FILE_VERSION)
