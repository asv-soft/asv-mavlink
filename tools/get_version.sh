#!/bin/zsh

git describe --long | sed -r "s|v([0-9]*)[.]([0-9]*)[.]([0-9]*)[-]([0-9]*).*|\1.\2.\3|g"