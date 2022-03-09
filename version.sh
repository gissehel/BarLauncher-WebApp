#!/usr/bin/env bash

VERSION="$(cat VERSION)"

MAJOR="${VERSION%%.*}"
MAJOR_COMP="${VERSION#*.}"

MINOR="${MAJOR_COMP%%.*}"
MINOR_COMP="${MAJOR_COMP#*.}"

RELEASE="${MINOR_COMP%%[-._]*}"

write_version() {
    local new_version="$1"
    echo "${new_version}" > VERSION
    echo "<Project><PropertyGroup><Version>${new_version}</Version></PropertyGroup></Project>" > Version.targets
    sed -i -e 's/"Version":.*,/"Version": "'"${new_version}"'",/' */plugin.json
}

new_release() {
    let new_release="${RELEASE}+1"
    let new_dev_release="${RELEASE}+2"
    local new_version="${MAJOR}.${MINOR}.${new_release}"
    local new_dev_version="${MAJOR}.${MINOR}.${new_dev_release}"
    write_version "${new_version}"
    #git add .
    #git commit -m "VERSION : ${new_version}"
    #git tag "v${new_version}"
    write_version "${new_dev_version}"
    #git add .
    #git commit -m "VERSION : Starting new developement version ${new_version}"
    #git push
    #git push --tags
}

ensure_version() {
    local version="${MAJOR}.${MINOR}.${RELEASE}"
    write_version "${version}"
}

case "${1}" in
    new_release)
        new_release
        ;;
    ensure_version)
        ensure_version
        ;;
    *)
        echo "No actions..."
        ;;

esac
