import { DefaultTheme, NavigationContainer } from "@react-navigation/native";
import React from "react";
import { StatusBar } from "react-native";
import { SafeAreaProvider } from "react-native-safe-area-context";
import { AppRoutes } from "./src/Navigation/AppRoutes.native";
import { nativeTheme } from "./src/theme/nativeTheme";

const appNavigationTheme = {
    ...DefaultTheme,
    colors: {
        ...DefaultTheme.colors,
        background: nativeTheme.colors.uiBg,
        card: nativeTheme.colors.uiNavbarBg,
        border: nativeTheme.colors.uiNavbarBorder,
        text: nativeTheme.colors.uiText,
        primary: nativeTheme.colors.brand700,
    },
};

function App() {
    return (
        <SafeAreaProvider>
            <StatusBar barStyle="dark-content" backgroundColor={nativeTheme.colors.uiBg} />
            <NavigationContainer theme={appNavigationTheme}>
                <AppRoutes />
            </NavigationContainer>
        </SafeAreaProvider>
    );
}

export default App;
