import React from "react";
import { ScrollView, Text, View } from "react-native";
import { SafeAreaView } from "react-native-safe-area-context";
import { NavBar } from "../Components/NavBar.native";
import { nativeStyles } from "../theme/nativeStyles";

export function RouteSetupManager() {
    return (
        <SafeAreaView style={nativeStyles.screen}>
            <NavBar />
            <ScrollView contentContainerStyle={nativeStyles.pageShell}>
                <View style={nativeStyles.panel}>
                    <Text style={nativeStyles.sectionTitle}>Route Setup</Text>
                    <Text style={nativeStyles.muted}>
                        This screen is fully native-styled. Add native route stops, ordering, and map integrations here.
                    </Text>
                </View>
            </ScrollView>
        </SafeAreaView>
    );
}
