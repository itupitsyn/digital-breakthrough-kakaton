"use client";

import { useAuth } from "@/contexts/authContext";
import { Button } from "flowbite-react";
import { FC } from "react";

export const LoginButton: FC = () => {
  const { isAuth, auth, isAuthenticating } = useAuth();

  return (
    <Button type="button" onClick={auth} isProcessing={isAuthenticating} disabled={isAuthenticating}>
      {!isAuth ? "Войти" : "Перевойти"}
    </Button>
  );
};
