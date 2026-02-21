import React from "react";
import { ScrollView, Text, View } from "react-native";
import { SafeAreaView } from "react-native-safe-area-context";
import { NavBar } from "../Components/NavBar.native";
import { nativeStyles } from "../theme/nativeStyles";

export function SavedRoutes() {
    return (
        <SafeAreaView style={nativeStyles.screen}>
            <NavBar />
            <ScrollView contentContainerStyle={nativeStyles.pageShell}>
                <View style={nativeStyles.panel}>
                    <Text style={nativeStyles.sectionTitle}>Saved Routes</Text>
                    <Text style={nativeStyles.muted}>
                        This native screen is now using React Native styles only. Add route persistence and list
                        rendering here next.
                    </Text>
                </View>
            </ScrollView>
        </SafeAreaView>
    );
}
