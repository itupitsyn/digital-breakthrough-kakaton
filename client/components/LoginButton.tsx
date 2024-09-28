"use client";

import { useAuth } from "@/contexts/authContext";
import { Button } from "flowbite-react";
import { FC } from "react";

export const LoginButton: FC = () => {
  const { isAuth, auth, token, isAuthenticating } = useAuth();

  return (
    <Button type="button" onClick={auth} isProcessing={isAuthenticating} disabled={isAuthenticating} title={token}>
      {!isAuth ? "Войти" : "Перевойти"}
    </Button>
  );
};
