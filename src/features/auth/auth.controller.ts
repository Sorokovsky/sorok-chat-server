import {
  Body,
  Controller,
  Delete,
  HttpCode,
  HttpStatus,
  Patch,
  Post,
  Res,
  UploadedFile,
  UseInterceptors,
} from "@nestjs/common";
import { AuthService } from "./auth.service";
import { GetUserDto } from "@contracts/dto/user/get-user.dto";
import { SwaggerFile } from "@decorators/swagger-file.decorator";
import { FileInterceptor } from "@nestjs/platform-express";
import {
  ApiBadRequestResponse,
  ApiCreatedResponse,
  ApiNoContentResponse,
  ApiNotFoundResponse,
} from "@nestjs/swagger";
import { ErrorDto } from "@contracts/dto/error.dto";
import {
  CreateUserDto,
  CreateUserDtoWithoutAvatar,
} from "@contracts/dto/user/create-user.dto";
import { Response } from "express";
import { Auth } from "@decorators/auth.decorator";
import { LoginDto } from "@contracts/dto/auth/login.dto";

@Controller("auth")
export class AuthController {
  constructor(private readonly authService: AuthService) {}

  @Post("register")
  @SwaggerFile(CreateUserDto)
  @UseInterceptors(FileInterceptor("avatar"))
  @ApiCreatedResponse({
    type: GetUserDto,
    description: "Created user successfully",
  })
  @ApiBadRequestResponse({
    description: "User already exists",
    type: ErrorDto,
  })
  public async register(
    @Res({ passthrough: true }) response: Response,
    @Body() user: CreateUserDtoWithoutAvatar,
    @UploadedFile() avatar?: Express.Multer.File,
  ): Promise<GetUserDto> {
    return await this.authService.register(response, user, avatar);
  }

  @Auth()
  @Delete("logout")
  @HttpCode(HttpStatus.NO_CONTENT)
  @ApiNoContentResponse({
    description: "Logged out successfully",
  })
  public async logout(
    @Res({ passthrough: true }) response: Response,
  ): Promise<void> {
    return await this.authService.logout(response);
  }

  @Patch("login")
  @HttpCode(HttpStatus.NO_CONTENT)
  @ApiNoContentResponse({
    description: "Logged in successfully",
  })
  @ApiNotFoundResponse({
    description: "User not found",
    type: ErrorDto,
  })
  @ApiBadRequestResponse({
    description: "Wrong password",
    type: ErrorDto,
  })
  public async login(
    @Res({ passthrough: true }) response: Response,
    @Body() loginDto: LoginDto,
  ): Promise<void> {
    return await this.authService.login(response, loginDto);
  }
}
