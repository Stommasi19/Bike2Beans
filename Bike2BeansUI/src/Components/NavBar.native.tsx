import { useNavigation } from "@react-navigation/native";
import type { NativeStackNavigationProp } from "@react-navigation/native-stack";
import React from "react";
import { Pressable, Text, View } from "react-native";
import type { RootStackParamList } from "../Navigation/types";
import { nativeStyles } from "../theme/nativeStyles";

type Navigation = NativeStackNavigationProp<RootStackParamList>;

const navItems: Array<{ label: string; route: keyof RootStackParamList }> = [
    { label: "Login", route: "Login" },
    { label: "Home", route: "Home" },
    { label: "Saved Routes", route: "SavedRoutes" },
];

export function NavBar() {
    const nav = useNavigation<Navigation>();

    return (
        <View style={nativeStyles.navbar}>
            <View style={nativeStyles.navbarList}>
                {navItems.map((item) => (
                    <Pressable
                        key={item.route}
                        onPress={() => nav.navigate(item.route)}
                        style={({ pressed }) => [
                            nativeStyles.navbarButton,
                            pressed && nativeStyles.navbarButtonPressed,
                        ]}
                    >
                        <Text style={nativeStyles.navbarButtonText}>{item.label}</Text>
                    </Pressable>
                ))}
            </View>
        </View>
    );
}

