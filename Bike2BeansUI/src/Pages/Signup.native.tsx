import { useNavigation } from "@react-navigation/native";
import type { NativeStackNavigationProp } from "@react-navigation/native-stack";
import React from "react";
import { Pressable, Text, TextInput, View } from "react-native";
import { SafeAreaView } from "react-native-safe-area-context";
import type { RootStackParamList } from "../Navigation/types";
import { nativeStyles } from "../theme/nativeStyles";
import { nativeTheme } from "../theme/nativeTheme";

type SignupNavigation = NativeStackNavigationProp<RootStackParamList, "Signup">;

export function Signup() {
    const nav = useNavigation<SignupNavigation>();

    return (
        <SafeAreaView style={nativeStyles.authScreen}>
            <View style={[nativeStyles.authCard, nativeStyles.authCardSignup]}>
                <Text style={nativeStyles.authTitle}>Welcome to Bike2Beans</Text>

                <View style={nativeStyles.fieldGroup}>
                    <Text style={nativeStyles.fieldLabel}>Name</Text>
                    <TextInput
                        placeholder="Name"
                        placeholderTextColor={nativeTheme.colors.uiInputPlaceholder}
                        style={nativeStyles.input}
                    />
                </View>

                <View style={nativeStyles.fieldGroup}>
                    <Text style={nativeStyles.fieldLabel}>Email</Text>
                    <TextInput
                        autoCapitalize="none"
                        keyboardType="email-address"
                        placeholder="Email"
                        placeholderTextColor={nativeTheme.colors.uiInputPlaceholder}
                        style={nativeStyles.input}
                    />
                </View>

                <View style={nativeStyles.fieldGroup}>
                    <Text style={nativeStyles.fieldLabel}>Username</Text>
                    <TextInput
                        autoCapitalize="none"
                        autoCorrect={false}
                        placeholder="username"
                        placeholderTextColor={nativeTheme.colors.uiInputPlaceholder}
                        style={nativeStyles.input}
                    />
                </View>

                <View style={nativeStyles.fieldGroup}>
                    <Text style={nativeStyles.fieldLabel}>Password</Text>
                    <TextInput
                        secureTextEntry
                        autoCapitalize="none"
                        placeholder="password"
                        placeholderTextColor={nativeTheme.colors.uiInputPlaceholder}
                        style={nativeStyles.input}
                    />
                </View>

                <View style={nativeStyles.buttonRow}>
                    <Pressable
                        onPress={() => nav.navigate("Home")}
                        style={({ pressed }) => [
                            nativeStyles.buttonBase,
                            nativeStyles.buttonPrimary,
                            pressed && nativeStyles.buttonPressed,
                        ]}
                    >
                        <Text style={nativeStyles.buttonTextPrimary}>Sign Up</Text>
                    </Pressable>

                    <Pressable
                        onPress={() => nav.navigate("Login")}
                        style={({ pressed }) => [
                            nativeStyles.buttonBase,
                            nativeStyles.buttonSecondary,
                            pressed && nativeStyles.buttonPressed,
                        ]}
                    >
                        <Text style={nativeStyles.buttonTextSecondary}>Back to Sign In</Text>
                    </Pressable>
                </View>

                <Pressable
                    style={({ pressed }) => [
                        nativeStyles.buttonBase,
                        nativeStyles.buttonGhost,
                        pressed && nativeStyles.buttonPressed,
                    ]}
                >
                    <Text style={nativeStyles.buttonTextGhost}>Sign Up With Google</Text>
                </Pressable>
            </View>
        </SafeAreaView>
    );
}

