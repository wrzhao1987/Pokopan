#!/bin/sh
rm -rf "$TARGET_BUILD_DIR/$PRODUCT_NAME.app/Data"
cp -Rf "$PROJECT_DIR/Data" "$TARGET_BUILD_DIR/$PRODUCT_NAME.app/Data"
if [ "$PLATFORM_NAME" == "iphonesimulator" ]
then cp -f "$PROJECT_DIR"/*.png "$TARGET_BUILD_DIR/$PRODUCT_NAME.app/"
                          fi
